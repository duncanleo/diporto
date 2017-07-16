using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Diporto.Database;
using Diporto.Models;
using Diporto.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace Diporto.Controllers {
  [Route("api/photos")]
  public class PlacePhotoController : Controller {
    const string bucketName = "diporto";
    private readonly DatabaseContext context;
    private IAmazonS3 S3Client;
    private UserManager<User> userManager;
    public PlacePhotoController(DatabaseContext context, UserManager<User> userManager, IAmazonS3 s3Client) {
      this.context = context;
      this.userManager = userManager;
      this.S3Client = s3Client;
    }

    [HttpPost]
    // Users can only submit local files.
    public async Task<IActionResult> Create(UploadPhotoViewModel model) { // This implementation because this is form file uploading.
      if (model.PlaceId == -1) {
        return BadRequest();
      }
      var place = context.Places.FirstOrDefault(p => p.Id == model.PlaceId);
      if (place == null) {
        return NotFound();
      }
      if (place.PlacePhotos == null) {
        place.PlacePhotos = new List<PlacePhoto>();
      }
      var photo = new PlacePhoto {
        Place = place,
        IsGooglePlacesImage = false,
        FileName = model.File.FileName
      };
      context.PlacePhotos.Add(photo);
      context.SaveChanges();
      var fileExt = model.File.ContentType.Split('/').Last();
      await S3Client.PutObjectAsync(new PutObjectRequest {
        BucketName = bucketName,
        Key = $"{photo.Id}-{model.File.FileName}",
        InputStream = model.File.OpenReadStream()
      });
      place.PlacePhotos.Add(photo);
      context.Places.Update(place);
      context.SaveChanges();
      return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) {
      var photo = context.PlacePhotos.FirstOrDefault(p => p.Id == id);
      if (photo == null) {
        return NotFound();
      }
      if (photo.IsGooglePlacesImage) {
        // TODO: Do something
        return null;
      } else {
        try {
          var s3Response = await S3Client.GetObjectAsync(bucketName, $"{id}-{photo.FileName}");
          return new FileStreamResult(s3Response.ResponseStream, "image/jpg");
        } catch (Exception e) {
          return NotFound();
        }
      }
    }
  }
}
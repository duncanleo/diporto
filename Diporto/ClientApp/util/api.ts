import 'whatwg-fetch';

export class DiportoApi {
  getPlace(id): Promise<Place> {
    const fetchTask = fetch(`/api/places/${id}`)
    .then(response => {
      return response.json() as Promise<Place>
    })

    return fetchTask;
  }
}

export default new DiportoApi();
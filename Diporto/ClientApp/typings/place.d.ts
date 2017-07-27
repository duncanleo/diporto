declare interface Place {
  id: number,
  name: string,
  lat: number,
  lon: number,
  phone: string,
  address: string,
  photos?: Photo[],
  categories: string[],
  reviews: Review[],
}
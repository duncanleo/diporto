declare interface ReviewSubmission {
  rating: number;
  text: string;
  place_id: number;
  time: string;
}

declare interface Review {
  rating: number;
  time: string;
  text: string;
  user?: User;
  id: number;
  place_id: number;
}
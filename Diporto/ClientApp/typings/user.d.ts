declare interface User {
  name: string,
  profile_image_url?: string
  reviews: Review[]
  user_name: string
  id: number
}

declare interface TokenResponse {
  token: string,
  refresh_token: string,
  expiration: string
}

declare interface UserRegistration {
  UserName: string,
  Password: string,
  ConfirmPassword: string,
  Email: string,
  Name: string
}
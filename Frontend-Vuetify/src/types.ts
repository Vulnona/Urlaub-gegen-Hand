export type ApiError = {
  message: string
}

export type User = {
  id: number;
  gender: string; // salutation?
  firstName: string;
  lastName: string;
  username: string;
  password?: string;
  street: string;
  houseNumber: string;
  zipCode: string;
  city: string;
  country: string;
  dateOfBirth: Date;
  email: string;
  facebookId: number;
  isFirstActivation: string;
  verified: boolean; // Personalausweis erfolgreich yes?no
  verificationState: VerificationState;
  membership?: boolean; // Current: hasMembership yes?no, Future: types of membership
  token: string
}

export type RegisterUser = {
  firstname: string;
  lastname: string;
  username: string;
  address: string;
  zipCode: string;
  city: string;
  state: string;
  country: string;
}

export type Offer = {
  id: string;
  image: string;
  title: string;
  description: string;
}

export type ImageString = string

export type Product = {
  id: number
  title: string
  description: string
  thumbnail: ImageString
  images: Array<ImageString>
}

export type Result = {
  products: Array<Product>
  total: number
  skip: number
  limit: number
}

export type UserVerification = {
  user: {
    id: string // do we have a userId already, when user sends register?
    firstName: string
    lastName: string
    email: string
  }
  coupon: {
    code: string
    isValid: boolean
  }
  state: VerificationState
}

export enum VerificationState {
  New = 'new',
  Pending = 'pending',
  Failed = 'failed',
  Verified = 'verified',
}

export enum PostVerificationState {
  new = 0,
  pending = 1,
  failed = 2,
  verified = 3,
}

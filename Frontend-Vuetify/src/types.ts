export type ApiError = {
  message: string
}

export type User = {
  id: number;
  gender: string; // salutation?
  firstName: string;
  lastName: string;
  username: string;
  nickName: string | null; // username !== nickname?
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
  verified: boolean; // ist das nicht obsolet siehe unten?
  verificationState: string; // welchen State kann der User haben
  membership?: string; // welche memberships gibt es
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
export type PriceInEuro = number

export type Product = {
  id: number
  title: string
  description: string
  price: PriceInEuro
  thumbnail: ImageString
  images: Array<ImageString>
}

export type Result = {
  products: Array<Product>
  total: number
  skip: number
  limit: number
}

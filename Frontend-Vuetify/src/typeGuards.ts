import {ApiError, User} from '@/types';

export const isApiError = (unknownType: unknown):  unknownType is ApiError => {
  return (
    unknownType !== null &&
    unknownType !== undefined &&
    (unknownType as ApiError).message !== undefined)
}

export const isUser = (unknownType: unknown):  unknownType is User => {
  return (
    unknownType !== null &&
    unknownType !== undefined &&
    (unknownType as User).id !== undefined &&
    (unknownType as User).firstName !== undefined &&
    (unknownType as User).lastName !== undefined &&
    (unknownType as User).token !== undefined)
}

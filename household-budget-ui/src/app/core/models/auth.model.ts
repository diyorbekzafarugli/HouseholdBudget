export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  userId: string;
  username: string;
  accessToken: string;
  refreshToken: string;
  accessTokenExpiry: string;
}
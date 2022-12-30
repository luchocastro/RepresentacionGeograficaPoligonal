export class User {
  id: string;
  username: string;
  password: string;
  role: string;
  token?: string;
  roleId: number;
  loginAttemps: number;
}

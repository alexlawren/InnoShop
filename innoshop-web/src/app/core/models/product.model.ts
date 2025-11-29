export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
  userId: string;
  createdAt: Date;
}
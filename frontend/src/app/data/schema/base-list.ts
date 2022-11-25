export class BaseList<T> {
  countTotal: number;
  pageNumber: number;
  pageSize: number;
  orderBy: string;
  list: T[];
}

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateUtilsService {
  /**
   * Convert long string date (ofen from Rest API) to a date string
   */
  getOnlyDate(dateStr: string | Date): string {
    const currentDate = new Date(dateStr);
    return currentDate.toISOString().substring(0, 10);
  }
}

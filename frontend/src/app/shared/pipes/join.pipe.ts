import { Pipe, PipeTransform } from '@angular/core';
import { isArray } from 'util';

@Pipe({
  name: 'arrayjoin',
})
export class JoinPipe implements PipeTransform {
  transform(input, character = ', ') {
    if (!isArray(input)) {
      return input;
    }

    return input.join(character);
  }
}

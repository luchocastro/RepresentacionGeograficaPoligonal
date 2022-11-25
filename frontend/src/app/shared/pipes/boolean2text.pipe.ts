import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'boolean2text',
})
export class Boolean2TextPipe implements PipeTransform {
  transform(input: boolean) {
    return input ? 'Sí' : 'No';
  }
}

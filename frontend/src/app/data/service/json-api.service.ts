import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JsonApiService {

  data = {
    projects: [
      {
        id: 1,
        thumbnail:
        'https://images.unsplash.com/photo-1520769669658-f07657f5a307?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1350&q=80',
        title: 'Project One'
      },
      {
        id: 2,
        thumbnail:
        'https://images.unsplash.com/photo-1521109762031-b71a005c25b7?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1351&q=80',
        title: 'Project Two'
      },
      {
        id: 3,
        thumbnail:
        'https://images.unsplash.com/photo-1531504060587-e6811129c0f2?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1351&q=80',
        title: 'Project Three'
      }
    ]
  };

  get(url: string): Observable<any> {

    switch (url) {
      case '/projects':
        return of(this.data.projects);
      default:
        const id = url.substring(url.lastIndexOf('/') + 1);
        return of(this.data.projects[id]);
    }
  }
}

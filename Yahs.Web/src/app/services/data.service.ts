import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private tiles = [
    { id: 1, title: 'Sales Overview', description: 'Monthly sales data', value: '$12,430' },
    { id: 2, title: 'User Growth', description: 'New users this month', value: '1,230' },
    { id: 3, title: 'Revenue', description: 'Year-to-date total', value: '$98,700' },
    { id: 4, title: 'Feedback', description: 'Customer feedback count', value: '85%' }
  ];

  getTiles() {
    return this.tiles;
  }

  getTileById(id: number) {
    return this.tiles.find(t => t.id === id);
  }
}

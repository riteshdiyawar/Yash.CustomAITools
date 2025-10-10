import { Component, AfterViewInit } from '@angular/core';
import { CommonModule, NgForOf } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import Chart from 'chart.js/auto';

import { RouterModule } from '@angular/router';
import { ProjectDetailComponent } from '../../project-detail/project-detail.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatTableModule,
    NgForOf,
    ProjectDetailComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements AfterViewInit {
  constructor(private router: Router) {}
  tiles = [
    { label: 'User 1', icon: 'people', count: 1280, color: 'linear-gradient(135deg, #2196f3, #21cbf3)' },
    { label: 'User 2', icon: 'person', count: 202, color: 'linear-gradient(135deg, #3949ab, #90caf9)' },
    { label: 'User 3', icon: 'person', count: 303, color: 'linear-gradient(135deg, #66bb6a, #43a047)' },
    { label: 'User 4', icon: 'person', count: 404, color: 'linear-gradient(135deg, #fbc02d, #fff176)' },
    { label: 'User 5', icon: 'person', count: 101, color: 'linear-gradient(135deg, #8e24aa, #ce93d8)' },
    { label: 'Active Sessions', icon: 'computer', count: 320, color: 'linear-gradient(135deg, #66bb6a, #43a047)' },
    { label: 'Info Logs', icon: 'info', count: 230, color: 'linear-gradient(135deg, #29b6f6, #0288d1)' },
    { label: 'Transactions', icon: 'receipt_long', count: 5400, color: 'linear-gradient(135deg, #ffa726, #fb8c00)' },
    { label: 'Errors', icon: 'error_outline', count: 12, color: 'linear-gradient(135deg, #e53935, #d32f2f)' },
    { label: 'Warnings', icon: 'warning_amber', count: 45, color: 'linear-gradient(135deg, #fdd835, #fbc02d)' },
  ];

  displayedColumns: string[] = ['id', 'name', 'status', 'date'];
  tableData = [
    { id: 1, name: 'John Doe', status: 'Active', date: '2025-10-01' },
    { id: 2, name: 'Jane Smith', status: 'Inactive', date: '2025-10-02' },
    { id: 3, name: 'Bob Johnson', status: 'Active', date: '2025-10-03' },
    { id: 4, name: 'Alice Brown', status: 'Pending', date: '2025-10-04' },
  ];

  ngAfterViewInit() {
    const ctx = (document.getElementById('barChart') as HTMLCanvasElement)?.getContext('2d');
    if (ctx) {
      new Chart(ctx, {
        type: 'bar',
        data: {
          labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
          datasets: [{
            label: 'Transactions',
            data: [12, 19, 15, 22, 18, 25, 20],
            backgroundColor: '#2196f3'
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: { display: false },
            title: { display: true, text: 'Transactions This Week' }
          }
        }
      });
    }
  }
  onTileClick(index: number) {
    if (index < 5) {
      this.router.navigate(['/tile-details', index+1]);
    }
    //index 6 naviagte to google
    else if (index === 5) {
      window.open('https://www.google.com', '_blank');
    }
  }
}
  
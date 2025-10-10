import { CommonModule, NgForOf } from '@angular/common';
import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tile-details',
  templateUrl: './tile-details.component.html',
  styleUrls: ['./tile-details.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatTableModule,
    NgForOf
  ]
})
export class TileDetailsComponent {
  tileId: number = 0;
  tile: any;

  tilesData = [
    {
      label: 'User 1',
      count: 1280,
      description: 'User 1 is a key member of the system, actively participating in various modules. High engagement and frequent dashboard usage.',
      status: 'Active',
      created: '2023-01-10',
      owner: 'John Doe',
      details: [
        { label: 'Last login', value: '2025-10-05' },
        { label: 'Role', value: 'Administrator' },
        { label: 'Recent activity', value: 'Updated profile, reviewed transactions.' },
        { label: 'Email', value: 'user1@example.com' }
      ]
    },
    {
      label: 'User 2',
      count: 202,
      description: 'User 2 is a regular user with moderate activity. Interacts mainly with reports and notifications.',
      status: 'Inactive',
      created: '2023-02-15',
      owner: 'Jane Smith',
      details: [
        { label: 'Last login', value: '2025-10-04' },
        { label: 'Role', value: 'User' },
        { label: 'Recent activity', value: 'Viewed info logs.' },
        { label: 'Email', value: 'user2@example.com' }
      ]
    },
    {
      label: 'User 3',
      count: 303,
      description: 'User 3 has shown increased activity, focusing on transaction management.',
      status: 'Active',
      created: '2023-03-20',
      owner: 'Alice Brown',
      details: [
        { label: 'Last login', value: '2025-10-03' },
        { label: 'Role', value: 'User' },
        { label: 'Recent activity', value: 'Managed transactions.' },
        { label: 'Email', value: 'user3@example.com' }
      ]
    },
    {
      label: 'User 4',
      count: 404,
      description: 'User 4 is a new member, exploring dashboard and system features.',
      status: 'Pending',
      created: '2023-04-05',
      owner: 'Bob Johnson',
      details: [
        { label: 'Last login', value: '2025-10-02' },
        { label: 'Role', value: 'User' },
        { label: 'Recent activity', value: 'Browsed dashboard.' },
        { label: 'Email', value: 'user4@example.com' }
      ]
    },
    {
      label: 'User 5',
      count: 101,
      description: 'User 5 mainly interacts with help and support sections.',
      status: 'Active',
      created: '2023-05-12',
      owner: 'Support Team',
      details: [
        { label: 'Last login', value: '2025-10-01' },
        { label: 'Role', value: 'Support' },
        { label: 'Recent activity', value: 'Accessed help section.' },
        { label: 'Email', value: 'user5@example.com' }
      ]
    },
    {
      label: 'Active Sessions',
      count: 320,
      description: 'Number of users currently logged in and using the system.',
      status: 'Ongoing',
      created: '2023-06-01',
      owner: 'System',
      details: [
        { label: 'Peak today', value: '350' },
        { label: 'Average session length', value: '45 min' },
        { label: 'Session type', value: 'Web, Mobile' }
      ]
    },
    {
      label: 'Info Logs',
      count: 230,
      description: 'System info logs provide insights into operations and user activities.',
      status: 'Normal',
      created: '2023-07-18',
      owner: 'System',
      details: [
        { label: 'Most recent log', value: '2025-10-05 14:22' },
        { label: 'Log type', value: 'System update' },
        { label: 'Log level', value: 'Info' }
      ]
    },
    {
      label: 'Transactions',
      count: 5400,
      description: 'Total transactions processed, including purchases, refunds, and adjustments.',
      status: 'High',
      created: '2023-08-22',
      owner: 'Finance',
      details: [
        { label: 'Highest single-day', value: '900' },
        { label: 'Last transaction', value: '2025-10-05 16:10' },
        { label: 'Transaction type', value: 'Purchase' }
      ]
    },
    {
      label: 'Errors',
      count: 12,
      description: 'System errors detected in the last 24 hours. Immediate attention may be required.',
      status: 'Critical',
      created: '2023-09-30',
      owner: 'Admin',
      details: [
        { label: 'Critical', value: '2' },
        { label: 'Minor', value: '10' },
        { label: 'Last error', value: '2025-10-05 15:30' }
      ]
    },
    {
      label: 'Warnings',
      count: 45,
      description: 'System warnings generated due to unusual activities or potential issues.',
      status: 'Warning',
      created: '2023-10-01',
      owner: 'System',
      details: [
        { label: 'Most common', value: 'Low disk space' },
        { label: 'Last warning', value: '2025-10-05 13:45' },
        { label: 'Warning type', value: 'Resource' }
      ]
    }
  ];

  constructor(private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.tileId = Number(params.get('id'));
      this.tile = this.tilesData[this.tileId-1] || null;
    });
  }
}
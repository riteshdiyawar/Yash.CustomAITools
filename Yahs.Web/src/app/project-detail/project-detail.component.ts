
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {  HttpClient,HttpParams } from '@angular/common/http';
// import { HttpClient } from '@angular/common/http';



 

// import { Component } from '@angular/core';

@Component({
  selector: 'app-project-detail',
  imports: [FormsModule],
  
 

  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent  
// app.component.ts

{
  baseurl: string='https://localhost:44335/api/YashCustomTool/';

  constructor(private http: HttpClient) {}

  projectPath: string = '';
  databaseConnection: string = '';
  projectTechnology: string = '';

  generateClassDiagram() {
    // Logic here
  }

  generateUnitTest() {
    // Logic here
  }

  generateDatabaseState() {
    // Logic here
  }

  generateFeatureAndPage() {
    // Logic here
  }


  generateProjectDiagram()
  {


const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);

      const url = this.baseurl +'GetProjectDiagram';

 
this.http.get(url, {
  params,
  responseType: 'blob'
}).subscribe(blob => {
  const a = document.createElement('a');
  const objectUrl = URL.createObjectURL(blob);
  a.href = objectUrl;
  a.download = 'Yash_CustomTools_ProjectDiagram.md';
  a.click();
  URL.revokeObjectURL(objectUrl);
});


 

  }
}
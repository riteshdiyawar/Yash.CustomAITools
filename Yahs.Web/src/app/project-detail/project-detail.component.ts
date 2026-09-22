// ...existing code...
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';

export interface CodeImprovement {
  methodName: string;
  // methodBody: string;
  severity: string;
  improvement: string;
  suggestion: string;
}

export interface ControlDetail {
  ControlName: string; 
  Count: string;
}

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatTableModule
  ],
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})

export class ProjectDetailComponent {
  baseurl: string = 'https://localhost:44335/api/YashCustomTool/';
  

  displayedColumns: string[] = ['methodName',  'severity', 'improvement', 'suggestion', 'action'];
  dataSource = new MatTableDataSource<CodeImprovement>([]);

  controlDisplayedColumns: string[] = ['controlName',  'count'];
  controlDataSource = new MatTableDataSource<ControlDetail>([]);


  constructor(private http: HttpClient) {}

  projectPath: string = '';
  databaseConnection: string = '';
  projectTechnology: string = '';
  feature: string = '';
  activeDiv: string = '';


showDiv(divName: string) {
    this.activeDiv = divName;
  }



  onFeatureChange(value: string) {
    this.feature = value;
console.log('onFeatureChange called with:', value);
    // optionally set a hard-coded project path for this feature
    // if (value === 'GenerateFeatureandPage') {
    //   this.projectPath = 'E:\Yash\Yash.BusinessLogicExtractor\SourceCode\FEATURE';
    // }

    // reset visible section
    this.showDiv('');
    this.projectTechnology = 'AspxNet'

    // trigger behavior for the selected feature
    if (value === 'ShowCodeImprovements') {
      this.projectPath = 'E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\Data';
    } else if (value === 'ShowControlDetails') {
      this.projectPath = 'E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\Controls';
    } else if (value === 'GenerateFeatureandPage') {
      this.projectPath = 'E:\\Yash\\Yash.BusinessLogicExtractor\\SourceCode\\FEATURE';
    } else if (value === 'GenerateProjectDetail') {
      this.projectPath = 'E:\\Project Applied\\Anchor.SuretyPortal';
    } else if (value === 'GenerateProjectDiagram') {
      this.projectPath = 'E:\\Project Applied\\Anchor.SuretyPortal';
    }
  }


  genrateSummary() {
    if (this.feature === 'ShowCodeImprovements') {
      this.getCodeImprovement();
    }
    else if (this.feature === 'GenerateProjectDiagram') {
      this.generateProjectDiagram();
    }
    else if (this.feature === 'GenerateProjectDetail') {
      this.generateProjectDetail();
    } 
    else if (this.feature === 'GenerateFeatureandPage') {
      this.generateFeatureAndPage();
    }
 
    else if (this.feature === 'ShowControlDetails') {       
      this.showControlDetails();
    }

    
    else 
      {
        alert('Please select a valid feature.');
      }


  }
  
  generateProjectDetail() {
    const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);
  // Call this method when the action button is clicked for project detail
    const url = this.baseurl + 'GetProjectDetails';
    this.DownloadMD(url, params, 'Yash_CustomTools_ProjectDetail.md');
  }

  getCodeImprovementDetail(element: CodeImprovement) {
    const params = new HttpParams()
           .set('MethodName', element.methodName)
           .set('ProjectPath', this.projectPath);
    const url = this.baseurl + 'GetCodeImprovementDetail';
    this.DownloadMD(url, params, 'Yash_CustomTools_MethodImprovement.md');
    
  }

  getCodeImprovement() {
    const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);
    
    this.showDiv('divCodeImprovement');
    const url = this.baseurl + 'GetCodeImprovement';
    this.DownloadJson(url, params, 'Yash_CustomTools_ProjectImprovements.md');
  }



  showControlDetails() 
  {
   const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);

      
      this.showDiv('divControlDetails');
    const url = this.baseurl + 'GetControlInfo';
    this.DownloadControlJson(url, params, 'Yash_CustomTools_ProjectImprovements.md');
  }

  generateClassDiagram() {}
  generateUnitTest() {}
  generateDatabaseState() {}
  generateFeatureAndPage() 
  {
    
      
      const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);

  
    const url = this.baseurl + 'GetProjectFeatureDetail';
    this.DownloadMD(url, params, 'Yash_CustomTools_GetProjectFeatureDetail.md');
  }

  generateProjectDiagram() {
    const params = new HttpParams()
      .set('ProjectPath', this.projectPath)
      .set('DatabaseConnection', this.databaseConnection)
      .set('ProjectTechnologyType', this.projectTechnology);

    const url = this.baseurl + 'GetProjectDiagram';
    this.DownloadMD(url, params, 'Yash_CustomTools_ProjectDiagram.md');
  }

  DownloadJson(url: string, params: HttpParams, fileName: string) 
  {  
    this.http.get<any>(url, { params, responseType: 'text' as 'json' }).subscribe({
      next: (data) => 
      {
        let parsedData: CodeImprovement[] = [];
        if (typeof data === 'string') {
          try {
            // Try parsing directly
            parsedData = JSON.parse(data);
          } catch (e1) {
            try {
              // Try unescaping and parsing
              const cleaned = data
                .replace(/\\r\\n/g, '')
                .replace(/\\n/g, '')
                .replace(/\\"/g, '"')
                .replace(/"{/g, '{')
                .replace(/}"/g, '}');
              parsedData = JSON.parse(cleaned);
            } catch (e2) {
              alert('Failed to parse JSON. Check backend response format.');
              console.error('Raw data:', data);
              return;
            }
          }
        } else {
          parsedData = data;
        }
        this.dataSource.data = parsedData;        
        alert(this.dataSource.data.length + ' items loaded successfully');
      },
      error: (error) => {
        console.error('Error fetching JSON:', error);
      }
    });
  }


  DownloadControlJson(url: string, params: HttpParams, fileName: string) 
  {  
    this.http.get<any>(url, { params, responseType: 'text' as 'json' }).subscribe({
      next: (data) => 
      {
        let parsedData: ControlDetail[] = [];
        if (typeof data === 'string') {
          try {
            // Try parsing directly
            parsedData = JSON.parse(data);
          } catch (e1) {
            try {
              // Try unescaping and parsing
              const cleaned = data
                .replace(/\\r\\n/g, '')
                .replace(/\\n/g, '')
                .replace(/\\"/g, '"')
                .replace(/"{/g, '{')
                .replace(/}"/g, '}');
              parsedData = JSON.parse(cleaned);
            } catch (e2) {
              alert('Failed to parse JSON. Check backend response format.');
              console.error('Raw data:', data);
              return;
            }
          }
        } else {
          parsedData = data;
        }
        this.controlDataSource.data = parsedData;        
        alert(this.controlDataSource.data.length + ' items loaded successfully');
      },
      error: (error) => {
        console.error('Error fetching JSON:', error);
      }
    });
  }



  DownloadMD(url: string, params: HttpParams, fileName: string) {
    this.http.get(url, {
      params,
      responseType: 'blob'
    }).subscribe(blob => {
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(blob);
      a.href = objectUrl;
      a.download = fileName;
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }
}
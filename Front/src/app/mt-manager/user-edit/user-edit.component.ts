import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { OperationResult } from 'src/app/models/operationResult';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @ViewChild('closeBtn', { static: false })
  closeBtn: ElementRef
  constructor(private apiService: ApiService, private http: HttpClient) { }

  ngOnInit() {
  }

  onDelete() {
    this.http.delete(this.apiService.apiURI + 'User/' + this.apiService.userForEdit.id)
      .toPromise()
      .then((res: OperationResult) => {
        if (res.succeeded) {
          this.closeBtn.nativeElement.click()
          this.apiService.getUsers()
        }
      })
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.http.put(this.apiService.apiURI + 'User', this.apiService.userForEdit)
        .toPromise()
        .then((res: OperationResult) => {
          if (res.succeeded) {
            this.closeBtn.nativeElement.click()
          }
        })
    }

  }

}

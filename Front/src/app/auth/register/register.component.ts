import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';
import { OperationResult } from 'src/app/models/operationResult';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  message: string
  constructor(private apiService: ApiService, private router: Router) { }
  @ViewChild('closeModal', { static: false })
  closeModal: ElementRef

  ngOnInit() {
  }
  onSubmit(form: NgForm) {
    this.message = ""
    if (form.valid) {
      this.apiService.register(form.value)
        .toPromise()
        .then((res: OperationResult) => {
          if (res.succeeded) {
            this.apiService.makeAlert("Вы успешно зарегистрировались!");
            this.apiService.login(form.value)
              .toPromise()
              .then((res: OperationResult) => {
                localStorage.setItem('token', String(res.data));
                this.closeModal.nativeElement.click()
                form.reset()
                this.apiService.logInText = "Выход"
              }
              )
          }
          else
          this.message = res.message
        })
    }
    else {
      if (form.value.Password) {
        if (form.value.Password.length < 4)
          this.message = 'Слишком короткий пароль'
      }
      else
        this.message = 'Введите пароль'

      if (!form.value.Email) {
        this.message = 'Введите email'
      }
      if (!form.value.UserName) {
        this.message = 'Введите логин'
      }
      else
        if (form.value.UserName.length < 3)
          this.message = 'Слишком короткий логин'
    }


  }
}

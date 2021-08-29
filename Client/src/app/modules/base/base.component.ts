import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm, NgModelGroup } from '@angular/forms';
import { User } from 'src/app/services/user/user';
import { UserService } from 'src/app/services/user/user.service';
@Component({
  selector: 'app-base',
  template: ` <div>ComponentA</div> `,
})
export class BaseComponent {
  view = 1;
  model = {};
  response: any;
  user = new User();
  constructor(public _userService: UserService) {
    this._userService.loadUser().then((response) => {
      this.user = response;
    });
  }

  // form-validation logic
  isValid(container: NgForm | NgModelGroup): boolean {
    const form = container as NgForm;
    const group = container as NgModelGroup;

    const formGroup = form.form ? form.form : group.control;

    let count = 0;

    for (const k in formGroup.controls) {
      if (!Object.prototype.hasOwnProperty.call(formGroup.controls, k)) {
        continue;
      }

      const control = formGroup.get(k) as FormControl;

      if (!control) {
        continue;
      }

      control.markAsTouched();

      count += this.countErrors(control.errors);
    }

    return count === 0;
  }

  private countErrors(errors: Record<string, any> | null): number {
    if (!errors) {
      return 0;
    }

    let count = 0;

    for (const k in errors) {
      if (!Object.prototype.hasOwnProperty.call(errors, k)) {
        continue;
      }

      if (k !== 'remote') {
        count++;
      }
    }

    return count;
  }
}

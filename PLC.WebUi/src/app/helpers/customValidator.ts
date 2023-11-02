import { AbstractControl , ValidatorFn} from "@angular/forms";

export function CustomValidator() : ValidatorFn {
    return (control: AbstractControl) => {
        const regex = /^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$/;
    
        if (regex.test(control.value)) {
          return null;
        }
    
        return { ipError: true };
      };
}
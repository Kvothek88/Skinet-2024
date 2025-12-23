import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { PaymentSummary } from '../models/order';

@Pipe({
  name: 'payment'
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken['payment_method_preview'] | PaymentSummary, ...args: unknown[]): unknown {
    if (value && 'card' in value) {
      const {brand, exp_month, exp_year, last4} = 
        (value as ConfirmationToken['payment_method_preview']).card!;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${String(exp_month).padStart(2, '0')}/${exp_year}`;
    } else if (value && 'last4' in value){
      const {brand, expMonth, expYear, last4} = 
        value as PaymentSummary;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${String(expMonth).padStart(2, '0')}/${expYear}`;      
    } else {
      return 'Unknown card';
    }
  }

}

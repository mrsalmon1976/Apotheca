import { ErrorHandler} from '@angular/core';

export class GlobalErrorHandler implements ErrorHandler {

    handleError(error) {
        alert('client error occurred');
        console.error(error);
    }
}

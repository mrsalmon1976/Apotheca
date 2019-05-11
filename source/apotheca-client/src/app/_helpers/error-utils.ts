export class ErrorUtils {

    public static getMessage(err: any) {
        if (err.name && err.status == 0 && err.name === 'HttpErrorResponse') {
            return 'Oh dear, the Apotheca API appears to be unavailable'
        }
        if (err.error) {
            if (err.error.message) {
                return err.error.message;
            }
            return err.error;
        }
        if (err.status) {
            return `An error occurred on the server: ${err.status} ${err.statusText}`;
        }        
        return 'An unspecified error occurred';
    }
}

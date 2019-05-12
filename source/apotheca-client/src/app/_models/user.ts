import { Store } from './store';

export class User {
    id: number;
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    token?: string;
    stores: Store[];
}
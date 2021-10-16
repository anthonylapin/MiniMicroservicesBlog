import { IComment } from './comment';

export interface IPost {
    id: number;
    title: string;
    comments: IComment[];
}
import React from 'react';
import { IComment } from '../../../../types/comment';

interface ICommentsListProps {
    comments: IComment[];
}

const CommentsList: React.FC<ICommentsListProps> = ({comments}) => {
    return (
        <div>
            <ul>
                {comments.map(c => (
                    <li key={c.id}>{c.content}</li>
                ))}
            </ul>
        </div>
    )
}

export default CommentsList;
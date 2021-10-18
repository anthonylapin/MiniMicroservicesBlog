import React from 'react';
import { IComment } from '../../../../types/comment';

interface ICommentsListProps {
    comments: IComment[];
}

const CommentsList: React.FC<ICommentsListProps> = ({comments}) => {
    return (
        <div>
            <ul>
                {comments.map(c => {
                    let content = c.content;

                    if (c.commentStatus === 'Pending') {
                        content = 'This comment is awaiting moderation';
                    }

                    if (c.commentStatus === 'Rejected') {
                        content = 'This comment has been rejected';
                    }

                    return (
                        <li key={c.id}>{content}</li>
                    )
                })}
            </ul>
        </div>
    )
}

export default CommentsList;
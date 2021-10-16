import React, { useState } from 'react';
import axios from 'axios';
import { createCommentUrl } from '../../../../constants/endpoints';

interface CommentCreateProps {
    postId: number;
}

const CommentCreate: React.FC<CommentCreateProps> = ({postId}) => {
    const [content, setContent] = useState('');

    const onContentChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setContent(e.target.value);
    }

    const onFormSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        await axios.post(createCommentUrl.replace('{0}', postId.toString()), {
            content
        });

        setContent('');
    }

    return (
        <div>
            <form onSubmit={onFormSubmit}>
                <div className="mb-3">
                    <label className="form-label">Title</label>
                    <input type="text" value={content} onChange={onContentChange} className="form-control" />
                </div>
                <button className="btn btn-primary">Create a comment</button>
            </form>
        </div>
    )
}

export default CommentCreate;
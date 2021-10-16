import React, { useState } from 'react';
import axios from 'axios';
import { createPostUrl } from '../../constants/endpoints';

const defaultTitleValue = '';

export function PostCreate() {
    const [title, setTitle] = useState(defaultTitleValue);

    const onTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(e.target.value);
    }

    const onFormSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        await axios.post(createPostUrl, {
            title
        });

        setTitle(defaultTitleValue);
    }

    return (
        <div>
            <form onSubmit={onFormSubmit}>
                <div className="mb-3">
                    <label className="form-label">Title</label>
                    <input value={title} onChange={onTitleChange} type="text" className="form-control"/>
                </div>
                <button className="btn btn-primary">Create a Post</button>
            </form>
        </div>
    )
}
import React from 'react';
import { IPost } from '../../../types/post';
import CommentCreate from './CommentCreate';
import CommentsList from './CommentsList';

interface IPostProps {
    post: IPost;
}

const Post: React.FC<IPostProps> = ({post}) => {
    return (
        <div className="card col m-2" key={post.id}>
            <div className="card-body">
                <h3>{post.title}</h3>
                <CommentsList comments={post.comments} />
                <CommentCreate postId={post.id} />
            </div>
        </div>
    )
}

export default Post;
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { getAllPosts } from '../../constants/endpoints';
import { IPost } from '../../types/post';
import Post from './Post';

export function PostList() {
    const [posts, setPosts] = useState<IPost[]>([]);

    const fetchAllPosts = async () => {
        const response = await axios.get<IPost[]>(getAllPosts);
        console.log(response.data);
        setPosts(response.data);
    }

    useEffect(() => {
        fetchAllPosts().then();
    }, []);



    return (
      <div className="mt-2 row">
          {posts.length === 0 ? <h3>No posts created yet</h3> : posts.map(p => (
              <Post key={p.id} post={p} />
          ))}
      </div>
    );
}
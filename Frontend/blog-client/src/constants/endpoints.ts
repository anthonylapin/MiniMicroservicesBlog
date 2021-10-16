export const postsServiceUrl = 'http://localhost:5000/api';
export const commentsServiceUrl = 'http://localhost:5001/api'
export const queryServiceUrl = 'http://localhost:5003/api'

export const createPostUrl = `${postsServiceUrl}/posts`;
export const getAllPosts = `${queryServiceUrl}/posts`;

export const createCommentUrl = `${commentsServiceUrl}/posts/{0}/comments`;
export const getAllCommentsForPostUrl = createCommentUrl;
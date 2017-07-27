import * as React from 'react';
import ReviewListItem from '../ReviewItem'

export interface ReviewListProps extends React.Props<any> {
  reviews: Review[]
  user?: User
}

const ReviewList: React.SFC<ReviewListProps> = ({ reviews, user }) => {
  return (
    <div id="review-list-container">
      {reviews.map(review => <ReviewListItem review={review} user={user}/>)}
    </div>
  )
}

export default ReviewList;
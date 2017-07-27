import * as React from 'react';
import * as moment from 'moment';
import StarRating from 'react-stars';

declare interface ReviewItemProps {
  review: Review
  user?: User
}

class ReviewItem extends React.Component<ReviewItemProps, {}> {
  private renderUserImage() {
    let user = this.props.review.user;

    if (user && user.profile_image_url !== null) {
      return <img src={user.profile_image_url} className="h4 br3"/>
    } else {
      return <img className="h3 br3" src="https://s3-us-west-1.amazonaws.com/jurvis/placeholder_profile.svg"/>
    }
  }

  render() {
    const { review } = this.props;
    const user = this.props.user || review.user;

    return (
      <div className="flex mb2">
	<div className="flex flex-column w3">
	  {this.renderUserImage()}
	</div>
	<div className="ml2 w-70">
	  <span className="fw6">
	    {user == null ? "Anonymous User" : user.name} said:
	  </span>
	  <StarRating
	    size={20}
	    edit={false}
	    value={review.rating}
	  />
	  <span>{moment(review.time, moment.ISO_8601).format("YYYY-MM-DD")}</span>
	  <p className="f5 lh-copy measure">{review.text}</p>
	</div>
      </div>
    );
  }
}

export default ReviewItem;
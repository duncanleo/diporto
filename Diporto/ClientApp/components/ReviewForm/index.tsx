import * as React from 'react';
import { HTMLProps } from 'react';
import * as moment from 'moment';
import StarRating from 'react-stars';

declare interface ReviewFormProps {
  placeId: number
  placeName: string
  onSubmitPressed:(review: ReviewSubmission) => void
}

declare interface ReviewFormState {
  text: string
  rating: number
}

class ReviewForm extends React.Component<ReviewFormProps, ReviewFormState> {
  constructor(props) {
    super(props);

    this.state = {
      text: '',
      rating: 0,
    }

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
    this.onRatingChanged = this.onRatingChanged.bind(this);
  }

  onSubmit(e) {
    e.preventDefault();
    this.props.onSubmitPressed({place_id: this.props.placeId, text: this.state.text, rating: this.state.rating, time: moment().toISOString()})
  }

  onChange(e) {
    this.setState({ [e.target.name] : e.target.value });
  }

  onRatingChanged(newRating) {
    console.log(newRating);
    this.onChange({target: {name: "rating", value: newRating}});
  }

  renderSubmitButton() {
    const buttonStyle = {
      alignSelf: 'flex-end',
      border: 0
    } as HTMLProps<HTMLAnchorElement>

    return (
	<button style={buttonStyle} className="f6 link dim br3 ph3 pv2 mb2 dib white bg-hot-pink right pointer">
	  Submit
	</button>
    )
  }

  renderTextBox() {
    return (
      <div className="pa3 bt b--black-10 flex flex-column">
	<label className="f6 b db mb2">Write a Review <span className="normal black-60">(optional)</span></label>
	<textarea id="review" name="text" className="db border-box hover-black w-100 measure ba b--black-20 pa2 br2 mb2" aria-describedby="comment-desc" onChange={this.onChange} value={this.state.text}/>
	<small id="comment-desc" className="f6 black-60">Written reviews go a long way to help other users!</small>
	{ this.state.text.length > 0 && this.renderSubmitButton() }
      </div>
    )
  }

  renderStartReviewTip() {
    return (
      <div className="pa3 bt b--black-10 center">
	<span className="f6 f5-ns lh-copy measure">
	  Start your review of {this.props.placeName}
	</span>
      </div>
    )
  }

  render() {
    return (
      <div>
	<div className="mw5 mw6-ns hidden ba b--black-10 mv4">
	  <div className="pa3 center">
	    <StarRating
	      size={50}
	      half={false}
	      value={this.state.rating}
	      onChange={this.onRatingChanged}
	    />
	  </div>
	  { this.state.rating == 0 ? this.renderStartReviewTip() : this.renderTextBox() }
	</div>
      </div>
    )
  }
}

export default ReviewForm;
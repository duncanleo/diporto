import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Api from '../util/api';
import CategoryList from './CategoryList';
import ReviewItem from './ReviewItem';

declare interface PlaceState {
  place?: Place
}

type PlaceProps = RouteComponentProps<{ id: number }>

export default class PlaceDisplay extends React.Component<PlaceProps, PlaceState> {
  constructor(props) {
    super(props);

    this.state = {
      place: undefined,
    };
  }

  componentWillMount() {
    const placeId = this.props.match.params.id;

    Api.getPlace(placeId)
      .then(place => this.setState({place: place}));
  }

  componentWillReceiveProps(nextProps: PlaceProps) {
    const placeId = nextProps.match.params.id;

    Api.getPlace(placeId)
      .then(place => this.setState({place: place}));
  }

  renderPlaceInformation() {
    const { place } = this.state;
    return (
      <div id="place-information-container" className="flex flex-column mw8 center">
	<div id="place-meta-container" className="mb4">
	  <h2 className="f2 lh-title">{place.name}</h2>
	  <CategoryList categories={place.categories}/>
	</div>
	<div id="place-images-container" className="flex mb4">
	  {place.photos.slice(0,4).map(photo => {
	    const imageUrl = `/api/photos/${photo.id}`;
	    const imageStyle = { backgroundImage: `url(${imageUrl}` };
	    return (
	      <div key={photo.id} className="w-25">
		<div className="aspect-ratio aspect-ratio--1x1">
		  <img className="bg-center cover aspect-ratio--object"
		    style={imageStyle}/>
		</div>
	      </div>
	    );
	  })}
	</div>
	<div id="remaining-meta-container" className="flex">
	  <div className="flex flex-column w-70">
	    <h3 className="f2 lh-copy ma0">Reviews</h3>
	    <div id="reviews-container">
	      {place.reviews.map(review => {
		return (
		  <ReviewItem
		    key={review.id}
		    review={review}
		  />
		);
	      })}
	    </div>
	  </div>
	  <div className="w-30">
	    <span>{place.phone}</span>
	    <address>{place.address}</address>
	  </div>
	</div>
      </div>
    )
  }

  public render() {
    return (
      <div id="place-container">
	{ this.state.place == null ?
	  <h2>Loading...</h2>
	  :
	  this.renderPlaceInformation()
	}
      </div>
    )
  }
}
import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Api from '../util/api';
import CategoryList from './CategoryList';
import ReviewItem from './ReviewItem';
import Map from './Map';
import PlacePin from './PlacePin';
import ReviewForm from './ReviewForm';
import BookmarkButton from './BookmarkButton'
import { CanvasOverlay, Marker } from 'react-map-gl';

declare interface PlaceState {
	place?: Place
	isAuthenticated: boolean
	bookmark: Bookmark
}

type PlaceProps = RouteComponentProps<{ id: number }>

export default class PlaceDisplay extends React.Component<PlaceProps, PlaceState> {
  constructor(props) {
    super(props);

    this.state = {
			place: undefined,
			isAuthenticated: localStorage.getItem('id_token') != undefined,
			bookmark: undefined
		};

		this.submitReview = this.submitReview.bind(this);
		this.handleBookmarkButtonClicked = this.handleBookmarkButtonClicked.bind(this);
  }

	componentWillMount() {
    const placeId = this.props.match.params.id;

    Api.getPlace(placeId)
			.then(place => {
				this.setState({place: place})
			});

		Api.getBookmarks()
			.then(bookmarks => {
				const bookmark = bookmarks.filter(bookmark => bookmark.place_id == placeId)[0];

				this.setState({bookmark})
			})
	}

  componentWillReceiveProps(nextProps: PlaceProps) {
    const placeId = nextProps.match.params.id;

    Api.getPlace(placeId)
			.then(place => {
				this.setState({place: place})
			});

		Api.getBookmarks()
			.then(bookmarks => {
				const bookmark = bookmarks.filter(bookmark => bookmark.place_id == placeId)[0];

				this.setState({bookmark})
			})
  }

	submitReview(review: ReviewSubmission) {
		Api.submitReview(review)
			.then(response => {
				Api.getReviewsWithPlaceId(this.props.match.params.id)
					.then(reviews => this.state.place.reviews = reviews)
			})
			.catch(error => {
				alert(error)
			});
	}

	handleBookmarkButtonClicked() {
		const { place, bookmark } = this.state;
		if (bookmark) {
			Api.deleteBookmark(bookmark.id)
				.then(_ => this.setState({bookmark: undefined}))
				.catch(error => alert(error));
		} else {
			Api.submitBookmark(this.state.place.id)
				.then(bookmark => {
					this.setState({bookmark: bookmark})
				});
		}
	}

  renderPlaceInformation() {
    const { place, isAuthenticated, bookmark } = this.state;
    return (
      <div id="place-information-container" className="flex flex-column mw8 center">
				<div id="place-meta-container" className="mb3 flex items-center">
					<div>
						<h2 className="f2 lh-title mv3">{place.name}</h2>
						<CategoryList categories={place.categories}/>
					</div>
					<div style={{flexGrow: 1}}></div>
					{isAuthenticated &&
						<div>
							<BookmarkButton
								bookmarked={bookmark != null}
								onClick={this.handleBookmarkButtonClicked}
							/>
						</div>
					}
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
						<h3 className="f2 lh-title ma0 mb2">Reviews</h3>
						<div id="reviews-container">
							{isAuthenticated &&
								<ReviewForm
									placeId={place.id}
									placeName={place.name}
									onSubmitPressed={(review) => {this.submitReview(review)}}
								/>
							}
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
						<Map
							viewport={{latitude: place.lat, longitude: place.lon, zoom: 13, width: 300, height: 300}}
							places={[place]}/>
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
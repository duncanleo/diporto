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
import { BounceLoader } from 'halogen';

declare interface PlaceState {
	place?: Place
	isAuthenticated: boolean
	bookmark: Bookmark
	mapContainerDimensions: MapDimensions
}

type PlaceProps = RouteComponentProps<{ id: number }>

export default class PlaceDisplay extends React.Component<PlaceProps, PlaceState> {
	private mapContainer: HTMLDivElement;

  constructor(props) {
    super(props);

    this.state = {
			place: undefined,
			isAuthenticated: localStorage.getItem('id_token') != undefined,
			bookmark: undefined,
			mapContainerDimensions: { height: 300, width: 300 }
		};

		this.submitReview = this.submitReview.bind(this);
		this.handleBookmarkButtonClicked = this.handleBookmarkButtonClicked.bind(this);
	}

	componentDidMount() {
		const height = this.mapContainer.clientHeight;
		const width = this.mapContainer.clientWidth;
		this.setState({ mapContainerDimensions: { height, width } });
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
					.then(reviews => {
						const newPlace = Object.assign({}, this.state.place, {reviews: reviews});
						this.setState({place: newPlace});
					})
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
		const { place, isAuthenticated, bookmark, mapContainerDimensions } = this.state;

    return (
			<div>
				<div id="place-information-container" className="flex flex-column bg-near-white ph6 mb3">
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
				</div>
				<div id="remaining-meta-container" className="flex ph6 center">
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
							{place.reviews.length == 0 &&
								<span className=" f5 lh-copy">There are no reviews available for this listing.</span>
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
					<div className="w-30" ref={(mapContainer) => this.mapContainer = mapContainer}>
						<Map
							viewport={{latitude: place.lat, longitude: place.lon, zoom: 10, height: mapContainerDimensions.height, width: mapContainerDimensions.width}}
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
      <div id="place-container" className="flex flex-column" style={{flexGrow: 1}}>
				{ this.state.place == null ?
					<div className="self-center flex items-start justify-center" style={{flexGrow: 1}}>
						<BounceLoader
							color="#273CFE"
							size="100px"
						/>
					</div>
					:
					 this.renderPlaceInformation()
				}
      </div>
    )
  }
}
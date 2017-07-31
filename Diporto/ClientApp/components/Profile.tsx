import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Api from '../util/api';
import ReviewItem from './ReviewItem';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import ReviewList from './ReviewList';
import PlaceList from './PlaceList';
import { BounceLoader } from 'halogen';
import 'react-tabs/style/react-tabs.css';

declare interface ProfileState {
  user?: User,
  reviews?: Review[],
  bookmarks?: Place[],
}

type ProfileProps = RouteComponentProps<{ id: string }>

export default class Profile extends React.Component<ProfileProps, ProfileState> {
  constructor(props) {
    super(props);

    this.state = {
      user: undefined,
      bookmarks: undefined,
    }

    this.parseJwt = this.parseJwt.bind(this);
  }

  componentWillMount() {
    const userId = this.props.match.params.id;

    Api.getUser(userId)
      .then(user => {
        this.setState({ user: user })
        return user
      })
      .then(user => {
        Api.getReviews(this.state.user.user_name)
          .then(reviews => { this.setState({reviews: reviews}) })

        if (localStorage.getItem('id_token') === null) { return }

        const loggedInUserId = this.parseJwt(localStorage.getItem('id_token')).sub

        if (loggedInUserId == this.state.user.id) {
          Api.getBookmarks()
            .then(bookmarks => {
              const places = bookmarks.map(bookmark => bookmark.place);
              this.setState({bookmarks: places})
            })
        }
      });
  }

  componentWillReceiveProps(nextProps: ProfileProps) {
    const userId = nextProps.match.params.id;

    Api.getUser(userId)
      .then(user => {
        this.setState({ user: user })
        return user
      })
      .then(user => {
        Api.getReviews(this.state.user.user_name)
          .then(reviews => { this.setState({reviews: reviews}) })

        if (localStorage.getItem('id_token') === null) { return }

        const loggedInUserId = this.parseJwt(localStorage.getItem('id_token')).sub

        if (loggedInUserId == this.state.user.id) {
          Api.getBookmarks()
            .then(bookmarks => {
              const places = bookmarks.map(bookmark => bookmark.place);
              this.setState({bookmarks: places})
            })
        }
      });
  }

  parseJwt(token) {
    let base64Url = token.split('.')[1];
    let base64 = base64Url.replace('-', '+').replace('_', '/');
    return JSON.parse(window.atob(base64));
  }

  renderProfileInformation() {
    const { user, reviews, bookmarks } = this.state;
    const profileUrl = (user.profile_image_url !== null) ? user.profile_image_url : "https://s3-us-west-1.amazonaws.com/jurvis/placeholder_profile.svg"

    return (
      <div className="flex flex-column">
        <div id="profile-meta-container" className="flex mv4">
          <img src={profileUrl} className="h4 h4 br3" />
          <div className="flex flex-column">
            <h2 className="f3 lh-title">{user.name}</h2>
            <div id="user-meta" className="flex">
              <span>{reviews == null ? 0 : reviews.length} Reviews</span>
              {bookmarks && <span>{bookmarks.length} Bookmark(s)</span>}
            </div>
          </div>
        </div>
        <div id="tab-container">
          <Tabs>
            <TabList>
              <Tab>Reviews</Tab>
              {bookmarks && <Tab>Bookmarks</Tab>}
            </TabList>

            <TabPanel>
              {reviews &&
                <ReviewList reviews={reviews} user={user} />
              }
            </TabPanel>
            {bookmarks &&
              <TabPanel>
                <PlaceList
                  places={bookmarks}
                />
              </TabPanel>
            }
          </Tabs>
        </div>
      </div>
    )
  }

  render() {
    return (
      <div id="profile-container" className="w-60 center">
        { this.state.user === undefined ? (
					<div className="self-center flex items-start justify-center" style={{flexGrow: 1}}>
						<BounceLoader
							color="#273CFE"
							size="100px"
						/>
					</div>
        ) : (
          this.renderProfileInformation()
        )}
      </div>
    )
  }
}
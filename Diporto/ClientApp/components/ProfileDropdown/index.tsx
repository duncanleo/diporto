import * as React from 'react';
import { Link } from 'react-router-dom';

interface ProfileDropdownProps extends React.Props<any> {
  user: User;
  onLogout?: () => void
}

interface ProfileDropdownState extends React.ComponentState {
  open: boolean;
}

class ProfileDropdown extends React.Component<ProfileDropdownProps, ProfileDropdownState> {
  constructor(props) {
    super(props);

    this.state = {
      open: false
    };

    this.handleClick = this.handleClick.bind(this);
    this.handleClose = this.handleClose.bind(this);
    this.handleLogoutPressed = this.handleLogoutPressed.bind(this);
  }

  handleClick(e) {
    this.setState({ open: !this.state.open });
  }

  handleClose(e) {
    this.setState({ open: false })
  }

  handleLogoutPressed(e) {
    e.preventDefault();
    this.props.onLogout();
  }

  render () {
    const { user } = this.props;
    const avatarStyles = "h2 w2 br-100"

    return (
      <div id="profile-dropdown-container">
        <div className="flex items-center" ref="target" onClick={this.handleClick}>
            {user.profile_image_url !== null ?
          <img
              src={user.profile_image_url}
              alt="profile photo"
              className={avatarStyles}/>
            :
          <img src="https://s3-us-west-1.amazonaws.com/jurvis/placeholder_profile.svg" alt="profile photo"
          className={avatarStyles}/>
            }
            <span className="ml2 lh-copy white">{user.name}</span>
            <a
          href="#"
          className="f6 link dim pl2 white"
          onClick={this.handleLogoutPressed}>
          Logout
            </a>
        </div>
      </div>
    )
  }
}

export default ProfileDropdown;
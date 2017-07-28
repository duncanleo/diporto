import * as React from 'react';
import { Link } from 'react-router-dom';
import ProfileImage from '../ProfileImage';

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
          <div className="h2 w2">
            <ProfileImage user={user}/>
          </div>
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
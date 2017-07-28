import * as React from 'react';
import { Link } from 'react-router-dom';

declare interface ProfileImageProps {
  user: User;
}

const ProfileImage: React.SFC<ProfileImageProps> = ({ user }) => {
  const profileImageUrl = (user && user.profile_image_url) ? user.profile_image_url : "https://s3-us-west-1.amazonaws.com/jurvis/placeholder_profile.svg";

  return (
    <Link to={`/profile/${user.user_name}`}>
      <img className="br3" src={profileImageUrl} />
    </Link>
  )
};

export default ProfileImage;

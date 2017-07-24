import 'whatwg-fetch';

export class DiportoApi {
  getPlace(id): Promise<Place> {
    const fetchTask = fetch(`/api/places/${id}`)
    .then(response => {
      return response.json() as Promise<Place>
    })

    return fetchTask;
  }
  login(creds: Credentials): Promise<TokenResponse> {
    const fetchTask = fetch('/api/token', {
      method: 'POST',
      headers: {
	'Content-Type': 'application/json'
      },
      body: JSON.stringify({
	UserName: creds.username,
	Password: creds.password,
	GrantType: "access_token"
      })
    })
    .then(response => response.json());

    return fetchTask;
  }
  getMe(token): Promise<User> {
    const config = {
      headers: { 'Authorization' : `Bearer ${token}` }
    }

    const fetchTask = fetch('/api/me', config)
      .then(response => response.json() as Promise<User>)

    return fetchTask;
  }
}

export default new DiportoApi();
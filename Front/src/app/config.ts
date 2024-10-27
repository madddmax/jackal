let config = {
    BaseApi: "http://localhost:5130/api/",
    HasDebug: true,
}
if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
    // production code
    config = {
        BaseApi: 'https://jackal.team/api/',
        HasDebug: false,
    };
}

export default config;

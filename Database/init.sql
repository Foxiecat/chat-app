-- Member
CREATE TABLE Member (
    Id UUID NOT NULL PRIMARY KEY,
    Username VARCHAR(10) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'offline',
    Created TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Updated TIMESTAMPTZ
);


-- Community
CREATE TABLE Community (
    Id UUID NOT NULL PRIMARY KEY,
    OwnerId UUID NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Created TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Updated TIMESTAMPTZ,

    FOREIGN KEY (OwnerId) REFERENCES Member(Id)
);

CREATE TABLE CommunityMember (
    MemberId UUID NOT NULL,
    CommunityId UUID NOT NULL,
    Nickname VARCHAR(10),
    JoinedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    PRIMARY KEY (MemberId, CommunityId),
    FOREIGN KEY (MemberId) REFERENCES Member(Id) ON DELETE CASCADE,
    FOREIGN KEY (CommunityId) REFERENCES Community(Id) ON DELETE CASCADE
);


-- Channel
CREATE TABLE Channel (
    Id UUID NOT NULL PRIMARY KEY,
    CommunityId UUID NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Position INT NOT NULL DEFAULT 0,
    Created TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Updated TIMESTAMPTZ,

    FOREIGN KEY (CommunityId) REFERENCES Community(Id) ON DELETE CASCADE
);


-- Message
CREATE TABLE Message (
    Id UUID NOT NULL PRIMARY KEY,
    ChannelId UUID NOT NULL,
    SenderId UUID NOT NULL,
    Content TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
    Created TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Updated TIMESTAMPTZ,

    FOREIGN KEY (ChannelId) REFERENCES Channel(Id) ON DELETE CASCADE,
    FOREIGN KEY (SenderId) REFERENCES Member(Id)
);


-- Indexes
CREATE INDEX idx_message_channel_created ON Message(ChannelId, Created DESC);
CREATE INDEX idx_message_sender ON Message(SenderId);
CREATE INDEX idx_channel_community ON Channel(CommunityId);
CREATE INDEX idx_community_member_member ON CommunityMember(MemberId);

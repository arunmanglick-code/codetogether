create table users(
	username varchar(50) not null primary key,
	password varchar(50) not null,
	enabled tinyint not null
);

create table authorities (
	username varchar(50) not null,
	authority varchar(50) not null,
	constraint fk_authorities_users foreign key(username) references users(username)
);
create unique index ix_auth_username on authorities (username,authority);

INSERT INTO `student_tracker`.`authorities`
VALUES
('user1', 'ROLE_EMPLOYEE'),
('user2', 'ROLE_MANAGER'),
('user2', 'ROLE_MANAGER'),
('user3', 'ROLE_EMPLOYEE'),
('user3', 'ROLE_MANAGER'),
('user3', 'ROLE_ADMIN');


INSERT INTO `student_tracker`.`authorities`
VALUES
('user1', 'ROLE_EMPLOYEE'),
('user2', 'ROLE_MANAGER'),
('user2', 'ROLE_MANAGER'),
('user3', 'ROLE_EMPLOYEE'),
('user3', 'ROLE_MANAGER'),
('user3', 'ROLE_ADMIN');


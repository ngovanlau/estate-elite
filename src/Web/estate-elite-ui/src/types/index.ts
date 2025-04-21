export interface Filters {
  keyword: string;
}

export interface Province {
  Code: string;
  Name: string;
  FullName: string;
  District: District[];
}

export interface District {
  Code: string;
  Name: string;
  FullName: string;
  Ward: Ward[];
}

export interface Ward {
  Code: string;
  Name: string;
  FullName: string;
}

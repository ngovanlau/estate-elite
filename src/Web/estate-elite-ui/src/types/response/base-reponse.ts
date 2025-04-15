export interface ApiResponse<T> {
    succeeded: boolean;
    message: string;
    code: string;
    data: T;
    errors: object;
}

export interface PageApiResponse<T> extends ApiResponse<T> {
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
    totalPages: number;
}

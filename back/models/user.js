import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const userSchema = new Schema(
    {
        username: {
            type: String,
            required: [true, 'Username is required'],
            minlength: [3, 'Username must be at least 3 characters long'],
            maxlength: [30, 'Username cannot exceed 30 characters'],
            match: [/^[a-zA-Z0-9]+$/, 'Username can only contain letters and numbers']
        },
        password: {
            type: String,
            required: [true, 'Password is required'],
            minlength: [6, 'Password must be at least 6 characters long'],
        },
        adresse: {
            type: String,
            required: [true, 'Adresse is required'],
            minlength: [5, 'Adresse must be at least 5 characters long'],
            match: [/^\S+@\S+\.\S+$/, 'Adresse must be a valid email address']
        },
        victoires: {
            type: Number,
            required: [true, 'Victoires is required'],
            min: [0, 'Victoires cannot be negative']
        },
        defaites: {
            type: Number,
            required: [true, 'Défaites is required'],
            min: [0, 'Défaites cannot be negative']
        }
    },
    {
        timestamps: true
    }
);

export default model('User', userSchema);

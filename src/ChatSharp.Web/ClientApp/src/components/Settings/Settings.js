import { useFormik } from 'formik';
import * as Yup from 'yup';
import clsx from 'clsx';
import { useEffect, useState } from 'react';
import { get, post } from '../../utils/HttpClient';
import LoadingSpinner from '../Utils/LoadingSpinner';
import { NavLink } from 'react-router-dom';

const settingsSchema = Yup.object().shape({
    modelsPath: Yup.string().when("enableLLM", {
        is: true,
        then: () => Yup.string().required("Models path is required")
    }),
    pathToSaveSessions: Yup.string().when("enableLLM", {
        is: true,
        then: () => Yup.string().required("Models save path is required")
    })
})

const Settings = () => {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const PopulateComponent = async () => {
            const settingsResponse = await get(`SettingsLoad`);

            if (settingsResponse.IsValid) {
                const settingsData = settingsResponse.Data;

                formik.setValues({
                    modelsPath: settingsData.ModelsPath,
                    enableLLM: settingsData.EnableLLM,
                    pathToSaveSessions: settingsData.PathToSaveSessions,
                });
            }

            setLoading(false);
        }

        PopulateComponent();
    }, []);

    const formik = useFormik({
        initialValues: {
            modelsPath: 'D:/LlmModels',
            pathToSaveSessions: 'D:/Models/Sessions',
            enableLLM: true,
        },
        validationSchema: settingsSchema,
        onSubmit: async (values, { setSubmitting }) => {

            try {
                setSubmitting(true);
                var postModel = {
                    ModelsPath: values.modelsPath,
                    EnableLLM: values.enableLLM,
                    PathToSaveSessions: values.pathToSaveSessions,
                };

                var result = await post('SettingsSave', postModel);
                setSubmitting(false);

                if (result.IsValid) {
                    //pnotify todo
                } else {
                    //todo
                }

            } catch (error) {
                setSubmitting(false);
            }
        },
    });

    return (
        <div className='settings-page'>
            {loading &&
                <LoadingSpinner />
            }

            {!loading &&
                <form className="form-horizontal"
                    autoComplete="off"
                    id="settings-form"
                    onSubmit={formik.handleSubmit}
                    noValidate>
                    <div className='settings-page-setting-block'>
                        <label className='settings-block-name'>Large Language Model</label>

                        <div className='settings-block-settings'>
                            <div className='settings-block-settings-setting cb-setting'>
                                <div className='cb-block'>
                                    <label>
                                        Enable
                                    </label>
                                    <span className='label-description'>
                                        Toggle to activate or deactivate the usage of a locally-hosted language processing model.
                                    </span>
                                </div>

                                <input
                                    name='enableLLM'
                                    type='checkbox'
                                    checked={formik.values.enableLLM}
                                    onChange={(e) => {
                                        formik.setFieldValue('enableLLM', e.target.checked);
                                    }}
                                />
                            </div>

                            <div className='settings-block-settings-setting input-setting'>
                                <label>
                                    Model path
                                </label>

                                <input
                                    {...formik.getFieldProps('modelsPath')}
                                    className={clsx(
                                        { 'is-invalid': formik.touched.modelsPath && formik.errors.modelsPath },
                                        {
                                            'is-valid': formik.touched.modelsPath && !formik.errors.modelsPath,
                                        }
                                    )}
                                    type='text'
                                    placeholder='Model path ex: D:/Models'
                                    autoComplete='off'
                                />

                                {formik.touched.modelsPath && formik.errors.modelsPath ? (
                                    <div className="error-message">{formik.errors.modelsPath}</div>
                                ) : null}
                            </div>

                            <div className='settings-block-settings-setting input-setting'>
                                <label>
                                    Sessions save path
                                </label>

                                <input
                                    {...formik.getFieldProps('pathToSaveSessions')}
                                    className={clsx(
                                        { 'is-invalid': formik.touched.pathToSaveSessions && formik.errors.pathToSaveSessions },
                                        {
                                            'is-valid': formik.touched.pathToSaveSessions && !formik.errors.pathToSaveSessions,
                                        }
                                    )}
                                    type='text'
                                    placeholder='Path to save ex: D:/Models/Sessions'
                                    autoComplete='off'
                                />

                                {formik.touched.pathToSaveSessions && formik.errors.pathToSaveSessions ? (
                                    <div className="error-message">{formik.errors.pathToSaveSessions}</div>
                                ) : null}
                            </div>
                        </div>
                    </div>

                    <div className='settings-page-footer-buttons'>
                        <NavLink className='btn-cancel' to='/'>Cancel</NavLink>
                        <button className='btn-submit' type='submit' disabled={formik.isSubmitting}>Save</button>
                    </div>
                </form>
            }
        </div>
    )
}

export default Settings;
